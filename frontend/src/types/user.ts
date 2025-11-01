export interface CustomProperty {
  id: number
  name: string
  value: string
}

export interface UserDto {
  id: number
  firstname: string
  lastname: string
  dateOfBirth: Date
  sex: 'Male' | 'Female'
  customProperties: CustomProperty[]
  title?: string
}

export interface CreateUpdateUserDto {
  firstname: string
  lastname: string
  dateOfBirth: Date | null
  sex: 'Male' | 'Female' | ''
  customProperties: CustomProperty[]
}
