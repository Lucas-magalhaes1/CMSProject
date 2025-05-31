import { useEffect, useState } from 'react'
import { useAuth } from '@/context/AuthContext'
import { useRouter } from 'next/router'
import axios from 'axios'

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

export default function TemplatesPage() {
  const { token, isAuthenticated } = useAuth()
  const [templates, setTemplates] = useState<Template[]>([])
  const router = useRouter()

  useEffect(() => {
    if (token === null) return
    if (!isAuthenticated) {
      router.push('/')
      return
    }

    const fetchTemplates = async () => {
      try {
        const response = await axios.get('http://localhost:8080/api/Templates', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        })

        setTemplates(response.data.dados)
        console.log('TEMPLATES CARREGADOS:', response.data.dados)
      } catch (error) {
        alert('Erro ao buscar templates')
      }
    }

    fetchTemplates()
  }, [token, isAuthenticated, router])

  if (!isAuthenticated) return null

  return (
    <div className="min-h-screen bg-gradient-to-b from-gray-100 to-gray-200 py-10 px-6">
      <div className="max-w-6xl mx-auto">
        <h1 className="text-3xl font-bold mb-8 text-center text-gray-800">
          Templates Disponíveis
        </h1>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {templates.map((template) => (
            <div
              key={template.id}
              className="bg-white rounded-2xl shadow-md p-6 hover:shadow-lg transition-shadow duration-300"
            >
              <h2 className="text-xl font-semibold text-blue-700 mb-4">
                {template.nome}
              </h2>

              <ul className="space-y-2">
                {template.campos.map((campo, index) => (
                  <li key={index} className="flex items-center justify-between">
                    <span className="text-gray-800">{campo.nome}</span>
                    <span
                      className={`text-xs font-medium px-2 py-1 rounded-full ${
                        campo.obrigatorio
                          ? 'bg-red-100 text-red-600'
                          : 'bg-green-100 text-green-600'
                      }`}
                    >
                      {campo.obrigatorio ? 'Obrigatório' : 'Opcional'}
                    </span>
                  </li>
                ))}
              </ul>

              <button
                onClick={() => router.push(`/conteudos/criar?id=${template.id}`)}
                className="mt-4 w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700 transition"
              >
                Usar este template
              </button>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
