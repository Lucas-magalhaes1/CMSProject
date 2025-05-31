import Link from 'next/link'
import { useAuth } from '@/context/AuthContext'
import { useEffect } from 'react'
import { useRouter } from 'next/router'

export default function AdminPage() {
  const { token, role, logout } = useAuth()
  const router = useRouter()

  useEffect(() => {
    if (token === null) return
    if (role !== 'Admin') router.push('/')
  }, [token, role, router])

  if (!token || role !== 'Admin') return null

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-100 to-gray-200 px-4 py-10">
      <div className="max-w-2xl mx-auto bg-white shadow-md rounded-2xl p-8">
        <div className="flex justify-between items-center mb-8">
          <h1 className="text-3xl font-bold text-gray-800">Painel do Administrador</h1>
          <button
            onClick={logout}
            className="text-sm text-red-600 underline hover:text-red-800"
          >
            Sair
          </button>
        </div>

        <div className="space-y-4">
          <Link href="/templates/criar">
            <button className="w-full bg-green-600 text-white py-3 rounded-xl font-medium hover:bg-green-700 transition">
              Criar Template
            </button>
          </Link>

          <Link href="/templates">
            <button className="w-full bg-blue-600 text-white py-3 rounded-xl font-medium hover:bg-blue-700 transition">
              Ver Templates
            </button>
          </Link>

          <Link href="/conteudos">
            <button className="w-full bg-purple-600 text-white py-3 rounded-xl font-medium hover:bg-purple-700 transition">
              Ver Meus Conteúdos
            </button>
          </Link>
        </div>
      </div>
    </div>
  )
}
