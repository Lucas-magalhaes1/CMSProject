import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { useAuth } from '@/context/AuthContext'
import axios from 'axios'
import { useForm } from 'react-hook-form'
import Link from 'next/link'

interface CampoPreenchido {
  nome: string
  valor: string
}

interface Conteudo {
  id: string
  titulo: string
  status: string
  templateId?: string
  camposPreenchidos: CampoPreenchido[]
  comentario?: string
}

export default function VisualizarConteudoPage() {
  const router = useRouter()
  const { id } = router.query
  const { token, role } = useAuth()
  const [conteudo, setConteudo] = useState<Conteudo | null>(null)
  const { register, handleSubmit, setValue } = useForm()

  const isEditable =
    role === 'Redator' &&
    conteudo &&
    (conteudo.status === 'Rascunho' || conteudo.status === 'Devolvido')

  useEffect(() => {
    if (!id || !token) return

    const fetch = async () => {
      try {
        const response = await axios.get(`http://localhost:8080/api/Conteudos/${id}`, {
          headers: { Authorization: `Bearer ${token}` },
        })

        const conteudo: Conteudo = response.data.dados || response.data
        setConteudo(conteudo)

        setValue('titulo', conteudo.titulo)
        conteudo.camposPreenchidos?.forEach((campo: CampoPreenchido) => {
          setValue(campo.nome, campo.valor)
        })
      } catch {
        alert('Erro ao carregar conteúdo')
        router.push('/conteudos')
      }
    }

    fetch()
  }, [id, token, router, setValue])

  const onSubmit = async (data: any) => {
    if (!conteudo) return

    const payload = {
      titulo: data.titulo,
      templateId: conteudo.templateId,
      camposPreenchidos: conteudo.camposPreenchidos.map((campo) => ({
        nome: campo.nome,
        valor: data[campo.nome] || '',
      })),
    }

    try {
      await axios.put(`http://localhost:8080/api/Conteudos/${conteudo.id}`, payload, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })

      alert('Conteúdo atualizado com sucesso!')
      router.push('/conteudos')
    } catch {
      alert('Erro ao atualizar conteúdo')
    }
  }

  if (!conteudo) return null

  return (
    <div className="min-h-screen bg-gray-100 py-10 px-4">
      <div className="max-w-2xl mx-auto bg-white shadow p-6 rounded">
        <h1 className="text-2xl font-bold mb-6 text-center">
          {isEditable ? 'Editar Conteúdo' : 'Visualizar Conteúdo'}
        </h1>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Título</label>
            <input
              {...register('titulo')}
              className="mt-1 w-full px-4 py-2 border rounded"
              disabled={!isEditable}
            />
          </div>

          {conteudo.camposPreenchidos?.map((campo, index) => (
            <div key={index}>
              <label className="block text-sm font-medium text-gray-700">{campo.nome}</label>
              <textarea
                {...register(campo.nome)}
                className="mt-1 w-full px-4 py-2 border rounded"
                disabled={!isEditable}
              />
            </div>
          ))}

          {conteudo.comentario && (
            <div className="bg-yellow-100 text-yellow-800 p-3 rounded border border-yellow-300">
              <strong>Comentário do editor:</strong> {conteudo.comentario}
            </div>
          )}

          {isEditable && (
            <button
              type="submit"
              className="w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700"
            >
              Salvar Alterações
            </button>
          )}
        </form>

        <Link href="/conteudos">
          <button className="mt-6 w-full bg-gray-300 text-gray-800 py-2 rounded hover:bg-gray-400">
            ← Voltar para a lista
          </button>
        </Link>
      </div>
    </div>
  )
}
