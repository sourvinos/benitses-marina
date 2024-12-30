import { Metadata } from '../../../../../shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface SupplierReadDto extends Metadata {

    id: number
    bank: SimpleEntity
    iban: string
    description: string
    longDescription: string
    vatNumber: string
    phones: string
    email: string
    remarks: string
    isActive: boolean
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
