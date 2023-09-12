export interface LoginData {
    email: string,
    password: string
  }

  export interface LoginResponse {
      responseType:number
      message :string
      responseObject :any
  }
  export interface RegisterResponse {
    success: boolean,
    message: string,
  }
