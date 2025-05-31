import { useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import { useForm, SubmitHandler } from 'react-hook-form'
import axios from 'axios'
import { useAuth } from '@/context/AuthContext'

interface CampoTemplate {
  nome: string
  tipo: number
  obrigatorio: boolean
}

interface Template {
  id: string
  nome: string
  campos: CampoTemplate[]
}

interface FormValues {
  titulo: string
  [key: string]: any
}

export default function CriarConteudoPage() {
  const router = useRouter()
  const { token } = useAuth()
  const [template, setTemplate] = useState<Template | null>(null)
  const { register, handleSubmit } = useForm<FormValues>()

  const templateId = router.query.id as string

  useEffect(() => {
    if (!templateId || !token) return

    const fetchTemplate = async () => {
      try {
        const response = await axios.get(`http://localhost:8080/api/Templates/${templateId}`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        })
        setTemplate(response.data.dados)
      } catch (error) {
        alert('Erro ao carregar o template')
        router.push('/templates')
      }
    }

    fetchTemplate()
  }, [templateId, token, router])

  const onSubmit: SubmitHandler<FormValues> = async (data) => {
    if (!template || !template.campos) {
      alert('Template inválido ou ainda não carregado.')
      return
    }

    const payload = {
      titulo: data.titulo,
      templateId: template.id,
      camposPreenchidos: template.campos.map((campo) => ({
        nome: campo.nome,
        valor: data[campo.nome] || '',
      })),
    }

    try {
      await axios.post('http://localhost:8080/api/Conteudos', payload, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })

      alert('Conteúdo criado com sucesso!')
      router.push('/conteudos')
    } catch (error) {
      alert('Erro ao criar conteúdo')
    }
  }

  if (!template) return null

  return (
    <div className="min-h-screen bg-gray-100 py-10 px-4">
      <div className="max-w-2xl mx-auto bg-white p-6 rounded shadow">
        <h1 className="text-2xl font-bold mb-6 text-center text-gray-800">
          Criar Conteúdo: {template.nome}
        </h1>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Título</label>
            <input
              {...register('titulo', { required: true })}
              className="mt-1 w-full px-4 py-2 border rounded"
            />
          </div>

          {template.campos?.map((campo, index) => (
            <div key={index}>
              <label className="block text-sm font-medium text-gray-700">
                {campo.nome} {campo.obrigatorio && '*'}
              </label>
              <input
                {...register(campo.nome, { required: campo.obrigatorio })}
                className="mt-1 w-full px-4 py-2 border rounded"
              />
            </div>
          ))}

          <button
            type="submit"
            className="w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700 transition"
          >
            Criar Conteúdo
          </button>
        </form>
      </div>
    </div>
  )
}
