import { useEffect, useState } from 'react'
import { useAuth } from '@/context/AuthContext'
import { useRouter } from 'next/router'
import axios from 'axios'
import Link from 'next/link'

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


export default function ListaConteudosPage() {
  const { token, isAuthenticated } = useAuth()
  const router = useRouter()
  const [conteudos, setConteudos] = useState<Conteudo[]>([])
  const [abertoId, setAbertoId] = useState<string | null>(null)

  useEffect(() => {
    if (token === null) return
    if (!isAuthenticated) {
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

        setConteudos(response.data.dados || response.data) // depende da estrutura da API
      } catch (error) {
        alert('Erro ao buscar conteúdos')
      }
    }

    fetchConteudos()
  }, [token, isAuthenticated, router])

  const excluirConteudo = async (id: string) => {
    if (!confirm('Tem certeza que deseja excluir este conteúdo?')) return

    try {
      await axios.delete(`http://localhost:8080/api/Conteudos/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      })

      alert('Conteúdo excluído com sucesso!')
      location.reload()
    } catch (error) {
      alert('Erro ao excluir conteúdo.')
    }
  }

  const submeterConteudo = async (id: string) => {
    try {
      console.log('Submetendo conteúdo ID:', id)

      await axios.post(
        `http://localhost:8080/api/AprovacaoConteudo/${id}/submeter`,
        { id }, // ainda é necessário o corpo com ID
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )

      alert('Conteúdo submetido com sucesso!')
      location.reload()
    } catch (error: any) {
      console.error('Erro ao submeter:', error.response?.data || error)
      alert('Erro ao submeter conteúdo')
    }
  }

  if (!isAuthenticated) return null

  return (
    <div className="min-h-screen bg-gray-100 py-10 px-4">
      <div className="max-w-5xl mx-auto">
        <h1 className="text-3xl font-bold mb-8 text-center">Meus Conteúdos</h1>

        <div className="space-y-4">
          {conteudos.length === 0 && (
            <p className="text-center text-gray-500">Nenhum conteúdo criado ainda.</p>
          )}

          {conteudos.map((conteudo) => (
            <div
              key={conteudo.id}
              className="bg-white p-4 rounded shadow space-y-2"
            >
              <div className="flex justify-between items-center">
                <div>
                  <h2 className="text-lg font-semibold">{conteudo.titulo}</h2>
                  <p className="text-sm text-gray-600">
                    Status:{" "}
                    {conteudo.status === 'Rascunho' ? (
                      <span className="font-semibold text-gray-700">Rascunho</span>
                    ) : conteudo.status === 'Submetido' ? (
                      <span className="font-semibold text-blue-700">Submetido</span>
                    ) : conteudo.status === 'Aprovado' ? (
                      <span className="font-semibold text-green-700">Aprovado</span>
                    ) : conteudo.status === 'Rejeitado' ? (
                      <span className="font-semibold text-red-700">Rejeitado</span>
                    ) : (
                      <span className="font-semibold">{conteudo.status}</span>
                    )}
                  </p>
                </div>

                <div className="flex gap-2">
                  <button
                    onClick={() => setAbertoId(abertoId === conteudo.id ? null : conteudo.id)}
                    className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
                  >
                    {abertoId === conteudo.id ? 'Fechar' : 'Visualizar'}
                  </button>

                  <button
                    onClick={() => excluirConteudo(conteudo.id)}
                    className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700"
                  >
                    Excluir
                  </button>

                  {conteudo.status === 'Rascunho' && (
                    <button
                      onClick={() => submeterConteudo(conteudo.id)}
                      className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
                    >
                      Submeter para aprovação
                    </button>
                  )}
                </div>
              </div>

              {abertoId === conteudo.id && (
                <div className="mt-3 px-4 py-3 bg-gray-50 rounded border border-gray-300 space-y-3">
                  <h3 className="text-md font-medium">Conteúdo Preenchido:</h3>

                  {conteudo.camposPreenchidos?.map((campo, index) => (
                    <div key={index}>
                      <strong>{campo.nome}:</strong> {campo.valor}
                    </div>
                  ))}

                  {conteudo.comentario && (
                    <div className="bg-yellow-100 text-yellow-800 p-3 rounded border border-yellow-300">
                      <strong>Comentário do editor:</strong> {conteudo.comentario}
                    </div>
                  )}

                  {(conteudo.status === 'Rascunho' || conteudo.status === 'Devolvido') && (
                    <Link href={`/conteudos/visualizar?id=${conteudo.id}`}>
                      <button className="mt-2 bg-gray-700 text-white px-4 py-2 rounded hover:bg-gray-800">
                        Editar
                      </button>
                    </Link>
                  )}
                </div>
              )}
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
