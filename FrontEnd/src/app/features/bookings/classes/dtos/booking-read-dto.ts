import { Guid } from 'guid-typescript'
// Custom
import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface BookingReadDto extends Metadata {

    bookingId: Guid
    boatType: SimpleEntity
    boatName: string
    boatLength: string
    fromDate: string
    toDate: string
    stayDuration: number
    contactDetails: string
    email: string
    remarks: string
    isConfirmed: boolean
    isDocked: boolean
    isPaid: boolean
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
