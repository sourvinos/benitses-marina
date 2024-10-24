import { Guid } from 'guid-typescript'
// Custom
import { Metadata } from 'src/app/shared/classes/metadata'

export interface ReservationLeaseReadDto extends Metadata {

    incuranceCompany: string
    policyEnds: string
    policyNo: string,
    reservationId: Guid

}
