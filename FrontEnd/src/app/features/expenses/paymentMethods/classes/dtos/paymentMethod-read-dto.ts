import { Metadata } from 'src/app/shared/classes/metadata'

export interface PaymentMethodReadDto extends Metadata {

    id: number
    description: string
    isCredit: boolean
    isActive: boolean
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
