import { Guid } from 'guid-typescript'
// Custom
import { Metadata } from 'src/app/shared/classes/metadata'

export interface ReservationLeaseReadDto extends Metadata {

    reservationId: Guid
    customer: string
    incuranceCompany: string
    policyEnds: string
    policyNo: string,

}
