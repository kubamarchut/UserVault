import { boot } from 'quasar/wrappers'
import axios from 'axios'

// Create your custom API instance
const api = axios.create({ 
  baseURL: process.env.DEV ? 'https://localhost:7130/api' : '/api/'
})

export default boot(({ app }) => {
  app.config.globalProperties.$axios = axios
  app.config.globalProperties.$api = api
})

export { api }