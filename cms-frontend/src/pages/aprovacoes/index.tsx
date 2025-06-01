import { useEffect, useState } from 'react'
import { useAuth } from '@/context/AuthContext'
import { useRouter } from 'next/router'
import axios from 'axios'

interface CampoPreenchido {
  nome: string
  valor: string
}

interface Conteudo {
  id: string
  titulo: string
  status: string
  comentario?: string
  camposPreenchidos: CampoPreenchido[]
}


export default function AprovacoesPage() {
  const { token, role, isAuthenticated } = useAuth()
  const router = useRouter()
  const [conteudos, setConteudos] = useState<Conteudo[]>([])
  const [conteudoAberto, setConteudoAberto] = useState<Conteudo | null>(null)


  useEffect(() => {
    if (token === null) return
    if (!isAuthenticated || role !== 'Editor') {
      router.push('/')
      return
    }

    const fetchConteudos = async () => {
      try {
        const response = await axios.get('http://localhost:8080/api/Conteudos', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        })

        const todos = response.data.dados || response.data
        const submetidos = todos.filter((c: Conteudo) => c.status === 'Submetido')
        setConteudos(submetidos)
      } catch (error) {
        alert('Erro ao buscar conteúdos')
      }
    }

    fetchConteudos()
  }, [token, isAuthenticated, role, router])

  const aprovar = async (id: string) => {
    try {
      await axios.post(
        `http://localhost:8080/api/AprovacaoConteudo/${id}/aprovar`,
        { id }, // inclui ID no corpo, se o backend exigir
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )

      alert('Conteúdo aprovado com sucesso!')
      location.reload()
    } catch (error: any) {
      console.error('Erro ao aprovar conteúdo:', error.response?.data || error)
      alert('Erro ao aprovar conteúdo')
    }
  }

  const rejeitar = async (id: string) => {
    let comentario = ''
    do {
      comentario = prompt('Motivo da rejeição (mínimo 5 caracteres):')?.trim() || ''
    } while (comentario.length < 5)

    try {
      await axios.post(
        `http://localhost:8080/api/AprovacaoConteudo/${id}/rejeitar`,
        JSON.stringify(comentario), // 👈 envia como string literal
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        }
      )

      alert('Conteúdo rejeitado!')
      location.reload()
    } catch (error: any) {
      console.error('Erro ao rejeitar conteúdo:', error.response?.data || error)
      alert('Erro ao rejeitar conteúdo')
    }
  }

  const devolver = async (id: string) => {
    let comentario = ''
    do {
      comentario = prompt('Comentário para correção (mínimo 5 caracteres):')?.trim() || ''
    } while (comentario.length < 5)

    try {
      await axios.post(
        `http://localhost:8080/api/AprovacaoConteudo/${id}/devolver`,
        JSON.stringify(comentario), 
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        }
      )

      alert('Conteúdo devolvido para correção!')
      location.reload()
    } catch (error: any) {
      console.error('Erro ao devolver conteúdo:', error.response?.data || error)
      alert('Erro ao devolver conteúdo')
    }
  }

  if (!isAuthenticated || role !== 'Editor') return null

  return (
    <div className="min-h-screen bg-gray-100 py-10 px-4">
      <div className="max-w-5xl mx-auto">
        <h1 className="text-3xl font-bold mb-8 text-center">Conteúdos Submetidos</h1>

        <div className="space-y-4">
          {conteudos.length === 0 && (
            <p className="text-center text-gray-500">Nenhum conteúdo submetido para aprovação.</p>
          )}

          {conteudos.map((conteudo) => (
            <div
              key={conteudo.id}
              className="bg-white p-4 rounded shadow"
            >
              <h2 className="text-lg font-semibold">{conteudo.titulo}</h2>
              <p className="text-sm text-gray-600 mb-2">Status: <strong>{conteudo.status}</strong></p>

              <div className="flex gap-3 mt-2">
                <button
                  onClick={() => aprovar(conteudo.id)}
                  className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
                >
                  Aprovar
                </button>
                <button
                  onClick={() => rejeitar(conteudo.id)}
                  className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700"
                >
                  Rejeitar
                </button>
                <button
                  onClick={() => devolver(conteudo.id)}
                  className="bg-yellow-500 text-white px-4 py-2 rounded hover:bg-yellow-600"
                >
                  Devolver para correção
                </button>
                <button
                  onClick={() => setConteudoAberto(conteudo)}
                  className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
                >
                  Visualizar
                </button>

                {conteudoAberto && (
                  <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
                    <div className="bg-white p-6 rounded shadow-lg max-w-xl w-full space-y-4 relative">
                      <h2 className="text-2xl font-bold mb-2">{conteudoAberto.titulo}</h2>
                      <p className="text-sm text-gray-500">Status: {conteudoAberto.status}</p>

                      <div className="space-y-2">
                        <h3 className="font-semibold text-gray-700">Campos preenchidos:</h3>
                        {conteudoAberto.camposPreenchidos?.map((campo, index) => (
                          <div key={index} className="text-sm">
                            <strong>{campo.nome}:</strong> {campo.valor}
                          </div>
                        ))}
                      </div>

                      {conteudoAberto.comentario && (
                        <div className="bg-yellow-100 text-yellow-800 p-3 rounded border border-yellow-300">
                          <strong>Comentário:</strong> {conteudoAberto.comentario}
                        </div>
                      )}

                      <button
                        onClick={() => setConteudoAberto(null)}
                        className="mt-4 bg-gray-600 text-white px-4 py-2 rounded hover:bg-gray-700"
                      >
                        Fechar
                      </button>
                    </div>
                  </div>
                )}
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
