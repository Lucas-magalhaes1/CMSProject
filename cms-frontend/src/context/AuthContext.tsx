import { createContext, useContext, useState, useEffect, ReactNode } from 'react'
import axios from 'axios'
import { useRouter } from 'next/router'

type UserRole = 'Admin' | 'Editor' | 'Redator'

interface AuthContextData {
  token: string | null
  role: UserRole | null
  login: (email: string, senha: string) => Promise<void>
  logout: () => void
  isAuthenticated: boolean
}

const AuthContext = createContext({} as AuthContextData)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(null)
  const [role, setRole] = useState<UserRole | null>(null)

  const router = useRouter()

  useEffect(() => {
    const savedToken = localStorage.getItem('token')
    const savedRole = localStorage.getItem('role') as UserRole
    if (savedToken) {
      setToken(savedToken)
      setRole(savedRole)
    }
  }, [])

  async function login(email: string, senha: string) {
    try {
      const response = await axios.post('http://localhost:8080/api/Auth/login', {
        email,
        senha,
      })

      const { token, papel } = response.data.dados

      setToken(token)
      setRole(papel)
      
      localStorage.setItem('token', token)
      localStorage.setItem('role', papel)

      if (papel === 'Admin') router.push('/admin')
      else if (papel === 'Editor') router.push('/editor')
      else if (papel === 'Redator') router.push('/redator')
      else router.push('/')
    } catch (err) {
      alert('Erro ao fazer login. Verifique email e senha.')
    }
  }

  function logout() {
    setToken(null)
    setRole(null)
    localStorage.removeItem('token')
    localStorage.removeItem('role')
    router.push('/')
  }

  return (
    <AuthContext.Provider
      value={{token, role, login, logout, isAuthenticated: !!token,}}
    >
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  return useContext(AuthContext)
}
