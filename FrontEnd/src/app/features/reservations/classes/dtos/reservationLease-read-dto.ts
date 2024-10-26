import { Guid } from 'guid-typescript'

export interface ReservationLeaseReadDto {

    reservationId: Guid
    insuranceCompany: string
    policyNo: string,
    policyEnds: string
    flag: string
    registryPort: string
    registryNo: string
    boatType: string
    boatUsage: string
    netAmount: number
    vatAmount: number
    grossAmount: number

}
