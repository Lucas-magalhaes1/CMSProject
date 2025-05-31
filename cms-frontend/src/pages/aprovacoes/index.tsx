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
  camposPreenchidos: CampoPreenchido[]
}

export default function AprovacoesPage() {
  const { token, role, isAuthenticated } = useAuth()
  const router = useRouter()
  const [conteudos, setConteudos] = useState<Conteudo[]>([])

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
      await axios.post(`http://localhost:8080/api/AprovacaoConteudo/${id}/aprovar`, {}, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })

      alert('Conteúdo aprovado!')
      location.reload()
    } catch {
      alert('Erro ao aprovar conteúdo')
    }
  }

  const rejeitar = async (id: string) => {
    const comentario = prompt('Motivo da rejeição:')
    if (!comentario) return

    try {
      await axios.post(`http://localhost:8080/api/AprovacaoConteudo/${id}/rejeitar`, {
        id,
        comentario,
      }, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })

      alert('Conteúdo rejeitado!')
      location.reload()
    } catch {
      alert('Erro ao rejeitar conteúdo')
    }
  }

  const devolver = async (id: string) => {
    const comentario = prompt('Comentário para devolução:')
    if (!comentario) return

    try {
      await axios.post(`http://localhost:8080/api/AprovacaoConteudo/${id}/devolver`, {
        id,
        comentario,
      }, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })

      alert('Conteúdo devolvido para correção!')
      location.reload()
    } catch {
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
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
