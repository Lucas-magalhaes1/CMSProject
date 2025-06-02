import { X } from 'lucide-react'
import { useEffect, useState } from 'react'
import axios from 'axios'
import { useAuth } from '@/context/AuthContext'

interface Notificacao {
  id: string
  titulo: string
  mensagem: string
  dataCriacao: string
  lida: boolean
}

interface Props {
  onClose: () => void
  refreshKey?: number
}


export default function NotificationPanel({ onClose, refreshKey }: Props) {
  const { token } = useAuth()
  const [notificacoes, setNotificacoes] = useState<Notificacao[]>([])

  const buscarNotificacoes = async () => {
    try {
      const response = await axios.get('http://localhost:8080/api/Notificacoes', {
        headers: { Authorization: `Bearer ${token}` },
      })

      setNotificacoes(response.data.dados || response.data)
    } catch (err) {
      alert('Erro ao buscar notificações')
    }
  }

  const marcarComoLida = async (id: string) => {
    try {
      await axios.post(
        `http://localhost:8080/api/Notificacoes/${id}/marcar-como-lida`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      )
      buscarNotificacoes()
    } catch (err) {
      alert('Erro ao marcar notificação como lida')
    }
  }

  useEffect(() => {
    buscarNotificacoes()
  }, [refreshKey])

  return (
    <div className="fixed top-0 right-0 w-full sm:w-96 h-full bg-white shadow-lg border-l border-gray-200 z-50 p-4 overflow-y-auto transition-transform">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-lg font-bold">Notificações</h2>
        <button onClick={onClose}>
          <X className="w-5 h-5 text-gray-700" />
        </button>
      </div>

      {notificacoes.length === 0 ? (
        <p className="text-gray-500 text-sm text-center">Nenhuma notificação por enquanto.</p>
      ) : (
        <ul className="space-y-4">
          {notificacoes.map((noti) => (
            <li
              key={noti.id}
              className={`p-3 border rounded-md ${
                noti.lida ? 'bg-gray-100' : 'bg-blue-50'
              } shadow-sm`}
            >
              <h3 className="font-semibold">{noti.titulo}</h3>
              <p className="text-sm text-gray-600">{noti.mensagem}</p>
              <p className="text-xs text-gray-400 mt-1">
                {new Date(noti.dataCriacao).toLocaleString()}
              </p>
              {!noti.lida && (
                <button
                  onClick={() => marcarComoLida(noti.id)}
                  className="mt-2 text-sm text-blue-600 hover:underline"
                >
                  Marcar como lida
                </button>
              )}
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
