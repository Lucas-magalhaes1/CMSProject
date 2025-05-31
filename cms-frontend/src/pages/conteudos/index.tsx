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
  templateId: string
  camposPreenchidos: CampoPreenchido[]
}

export default function ListaConteudosPage() {
  const { token, isAuthenticated } = useAuth()
  const router = useRouter()
  const [conteudos, setConteudos] = useState<Conteudo[]>([])

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
              className="bg-white p-4 rounded shadow flex justify-between items-center"
            >
              <div>
                <h2 className="text-lg font-semibold">{conteudo.titulo}</h2>
                <p className="text-sm text-gray-600">Status: <strong>{conteudo.status}</strong></p>
              </div>

              {conteudo.status === 'Rascunho' && (
                <button
                  onClick={() => submeterConteudo(conteudo.id)}
                  className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
                >
                  Submeter para aprovação
                </button>
              )}
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
