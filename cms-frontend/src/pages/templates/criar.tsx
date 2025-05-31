import { useForm, useFieldArray } from 'react-hook-form'
import { useAuth } from '@/context/AuthContext'
import { useRouter } from 'next/router'
import axios from 'axios'
import { useEffect } from 'react'

interface Campo {
  nome: string
  tipo: number
  obrigatorio: boolean
}

interface TemplateForm {
  nome: string
  campos: Campo[]
}

export default function CriarTemplatePage() {
  const { token, isAuthenticated } = useAuth()
  const router = useRouter()

  const { register, control, handleSubmit } = useForm<TemplateForm>({
    defaultValues: {
      nome: '',
      campos: [{ nome: '', tipo: 0, obrigatorio: false }],
    },
  })

  const { fields, append, remove } = useFieldArray({
    control,
    name: 'campos',
  })

  useEffect(() => {
    if (token === null) return
    if (!isAuthenticated) {
      router.push('/')
    }
  }, [token, isAuthenticated, router])

  const onSubmit = async (data: TemplateForm) => {
    try {
      console.log('Payload enviado:', data)
      await axios.post('http://localhost:8080/api/Templates', data, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })

      alert('Template criado com sucesso!')
      router.push('/templates')
    } catch (error) {
      alert('Erro ao criar template')
    }
  }

  if (!isAuthenticated) return null

  return (
    <div className="min-h-screen bg-gray-100 py-10 px-4">
      <div className="max-w-2xl mx-auto bg-white p-6 rounded shadow">
        <h1 className="text-2xl font-bold mb-6 text-center">Criar Template</h1>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          <div>
            <label className="block text-sm font-medium text-gray-700">Nome do Template</label>
            <input
              {...register('nome', { required: true })}
              className="mt-1 w-full border px-4 py-2 rounded"
            />
          </div>

          <div>
            <h2 className="text-lg font-semibold mb-2">Campos</h2>
            {fields.map((field, index) => (
              <div
                key={field.id}
                className="bg-gray-50 p-4 mb-3 border rounded space-y-2"
              >
                <input
                  {...register(`campos.${index}.nome`, { required: true })}
                  placeholder="Nome do campo"
                  className="w-full px-3 py-2 border rounded"
                />

                <select
                  {...register(`campos.${index}.tipo`, {
                    required: true,
                    valueAsNumber: true,
                  })}   
                  className="w-full px-3 py-2 border rounded"
                >
                  <option value={0}>Texto Curto</option>
                  <option value={1}>Texto Médio</option>
                  <option value={2}>Texto Longo</option>
                </select>

                <label className="flex items-center space-x-2 text-sm">
                  <input
                    type="checkbox"
                    {...register(`campos.${index}.obrigatorio`)}
                    className="h-4 w-4"
                  />
                  <span>Obrigatório</span>
                </label>

                {fields.length > 1 && (
                  <button
                    type="button"
                    onClick={() => remove(index)}
                    className="text-red-600 text-sm underline"
                  >
                    Remover campo
                  </button>
                )}
              </div>
            ))}

            <button
              type="button"
              onClick={() =>
                append({ nome: '', tipo: 0, obrigatorio: false })
              }
              className="mt-2 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition"
            >
              Adicionar Campo
            </button>
          </div>

          <button
            type="submit"
            className="w-full bg-green-600 text-white py-2 px-4 rounded hover:bg-green-700 transition"
          >
            Criar Template
          </button>
        </form>
      </div>
    </div>
  )
}
