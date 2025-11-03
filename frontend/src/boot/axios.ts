import { boot } from 'quasar/wrappers'
import axios from 'axios'

const api = axios.create({ 
  baseURL: process.env.DEV ? 'https://localhost:7130/api' : '/api/',
  withCredentials: true
})

export default boot(async ({ app }) => {
  console.log("test")
  const response = await api.get('/users/csrf-token')
  const token = response.data.token

  api.defaults.headers.common['X-XSRF-TOKEN'] = token

  app.config.globalProperties.$axios = axios
  app.config.globalProperties.$api = api
})

export { api }