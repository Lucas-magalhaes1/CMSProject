import { useEffect, useState } from 'react'
import axios from 'axios'
import Head from 'next/head'

interface CampoPreenchido {
  nome: string
  valor: string
}

interface Conteudo {
  id: string
  titulo: string
  camposPreenchidos: CampoPreenchido[]
  dataPublicacao?: string
}

export default function NoticiasPage() {
  const [noticias, setNoticias] = useState<Conteudo[]>([])

  useEffect(() => {
    const fetchNoticias = async () => {
      try {
        const response = await axios.get('http://localhost:8080/api/public/conteudos')
        setNoticias(response.data.dados || response.data)
      } catch (error) {
        console.error('Erro ao carregar notícias', error)
      }
    }

    fetchNoticias()
  }, [])

  return (
    <>
      <Head>
        <title>Portal de Notícias</title>
      </Head>

      <div className="min-h-screen bg-gray-100 py-10 px-4">
        <div className="max-w-4xl mx-auto">
          <h1 className="text-4xl font-bold text-gray-800 mb-10 border-b pb-4 text-center">
            Portal de Notícias
          </h1>

          {noticias.length === 0 ? (
            <p className="text-center text-gray-500">Nenhuma notícia publicada ainda.</p>
          ) : (
            <div className="space-y-12">
              {noticias.map((noticia) => {
                const lead = noticia.camposPreenchidos.find((c) => c.nome.toLowerCase() === 'lead')
                const data = noticia.dataPublicacao || new Date().toLocaleDateString()

                return (
                  <div key={noticia.id} className="border-b pb-6">
                    <h2 className="text-2xl font-semibold text-gray-900">{noticia.titulo}</h2>
                    <p className="text-sm text-gray-500 mb-2">Publicado em {data}</p>
                    <div className="text-gray-700 text-base space-y-2">
                        {noticia.camposPreenchidos.map((campo) => (
                            <div key={campo.nome}>
                            <strong className="block">{campo.nome}:</strong>
                            <p>{campo.valor}</p>
                            </div>
                        ))}
                    </div>
                  </div>
                )
              })}
            </div>
          )}
        </div>
      </div>
    </>
  )
}
