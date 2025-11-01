import { Dark } from 'quasar'

export default () => {
  Dark.set(window.matchMedia('(prefers-color-scheme: dark)').matches)
}