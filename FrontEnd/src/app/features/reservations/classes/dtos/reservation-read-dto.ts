import { Guid } from 'guid-typescript'
// Custom
import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { PierReadDto } from './pier-read-dto'

export interface ReservationReadDto extends Metadata {

    reservationId: Guid
    boatName: string
    boatType: SimpleEntity
    length: string
    fromDate: string
    toDate: string
    days: number
    email: string
    remarks: string
    isConfirmed: boolean
    isDocked: boolean
    isPaid: boolean
    piers: PierReadDto[]
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
