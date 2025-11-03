import { boot } from 'quasar/wrappers'
import axios from 'axios'

// Create your custom API instance
const api = axios.create({ 
  baseURL: process.env.DEV ? 'https://localhost:7130/api' : '/api/',
  withCredentials: true
})

export default boot(async ({ app }) => {
  try {
    const res = await api.get('/csrf-token')
    const token = res.data.token

    api.interceptors.request.use(config => {
      const method = config.method?.toLowerCase() ?? ''
      if (['post', 'put', 'delete'].includes(method)) {
        config.headers['X-XSRF-TOKEN'] = token
      }
      return config
    })

  } catch (err) {
    console.error('Failed to fetch CSRF token', err)
  }
  app.config.globalProperties.$axios = axios
  app.config.globalProperties.$api = api
})


export { api }