import { Metadata } from '../../../../shared/classes/metadata'

export interface SupplierReadDto extends Metadata {

    // PK
    id: number
    // Fields
    description: string
    vatNumber: string
    phones: string
    email: string
    remarks: string
    isActive: boolean
    // Metadata
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
