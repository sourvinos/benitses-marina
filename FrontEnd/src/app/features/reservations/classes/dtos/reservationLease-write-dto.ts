import { Guid } from 'guid-typescript'
// Custom
import { Metadata } from 'src/app/shared/classes/metadata'

export interface ReservationLeaseWriteDto extends Metadata {

    reservationId: Guid
    incuranceCompany: string
    policyEnds: string
    policyNo: string,

}
