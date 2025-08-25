import type { AxiosInstance, AxiosRequestConfig } from 'axios'
import Axios from 'axios'

const baseURL = 'https://localhost:7217/api/'

const axios: AxiosInstance = Axios.create({
  baseURL,
  validateStatus:  () => true,
  withCredentials: true,
})

export interface Payload<T> {
  errors:   string | null
  payload:  T | null
  headers?: any
}

/**
 * Axios wrapper for ajax requests.
 * @param config Axios config object
 * @returns Promise with payload and errors objects.
 */

export async function fetchy<T = any>(
  config: AxiosRequestConfig
): Promise<Payload<T>> {
  let errors: string | null = null
  let payload: T | null = null
  let headers: any = null

  const genericError = 'Something went wrong'

  try {
    const response = await axios( config )
    const status = response.status
    const statusOk = Math.floor( status / 100 ) === 2

    if ( !statusOk ) {
      if ( response.data.errors ) {
        errors = response.data.errors.join( ', ' )
      }
      else if ( response.data.message && !response.data.details ) {
        errors = response.data.message
      }
      else if ( response.data.message && response.data.details ) {
        errors = [ response.data.message, response.data.details.join( ', ' ) ].join(
          ','
        )
      }
      else if ( typeof response.data === 'string' && response.data ) {
        errors = response.data
      }
      else {
        errors = genericError
      }
      payload = null
      headers = response.headers
    }
    else if ( response.data.errors ) {
      errors = response.data.errors.join( ', ' )
      payload = null
      headers = response.headers
    }
    else {
      errors = null

      payload = response.data
      headers = response.headers
    }

    return {
      errors,
      payload,
      headers,
    }
  }
  catch ( ex: string | any ) {
    return {
      errors:  ex.message,
      payload: null,
      headers: null,
    }
  }
}
