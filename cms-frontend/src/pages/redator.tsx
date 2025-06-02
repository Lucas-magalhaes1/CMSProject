import Link from 'next/link'
import { useAuth } from '@/context/AuthContext'
import { useEffect } from 'react'
import { useRouter } from 'next/router'
import { useState } from 'react'
import { Bell } from 'lucide-react'
import NotificationPanel from '@/components/NotificationPanel'

export default function RedatorPage() {
  const { token, role, logout } = useAuth()
  const router = useRouter()
  const [mostrarNotificacoes, setMostrarNotificacoes] = useState(false)
  const [notificacaoRefreshKey, setNotificacaoRefreshKey] = useState(0)

  useEffect(() => {
    if (token === null) return
    if (role !== 'Redator') router.push('/')
  }, [token, role, router])

  const abrirPainel = () => {
    setNotificacaoRefreshKey((prev) => prev + 1)
    setMostrarNotificacoes(true)
  }

  if (!token || role !== 'Redator') return null

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-100 to-gray-200 px-4 py-10">
      {/* Header */}
      <header className="max-w-4xl mx-auto flex justify-between items-center p-4 bg-white shadow-md rounded-xl mb-8">
        <h1 className="text-2xl md:text-3xl font-bold text-gray-800">Painel do Redator</h1>

        <div className="flex items-center gap-4">
          <button onClick={abrirPainel}>
            <Bell className="w-6 h-6 text-gray-700 hover:text-blue-600" />
          </button>

          <button
            onClick={logout}
            className="text-sm text-red-600 underline hover:text-red-800"
          >
            Sair
          </button>
        </div>
      </header>

      {mostrarNotificacoes && (
        <NotificationPanel
          onClose={() => setMostrarNotificacoes(false)}
          refreshKey={notificacaoRefreshKey}
        />
      )}

      {/* Conteúdo principal */}
      <main className="max-w-2xl mx-auto bg-white shadow-md rounded-2xl p-8">
        <h2 className="text-2xl font-semibold text-gray-800 mb-6 text-center">
          Ações disponíveis
        </h2>

        <div className="space-y-4">
          <Link href="/templates">
            <button className="w-full bg-blue-600 text-white py-3 rounded-xl font-medium hover:bg-blue-700 transition">
              Criar Conteúdo com Template
            </button>
          </Link>

          <Link href="/conteudos">
            <button className="w-full bg-purple-600 text-white py-3 rounded-xl font-medium hover:bg-purple-700 transition">
              Ver Meus Conteúdos
            </button>
          </Link>
        </div>
      </main>
    </div>
  )
}
