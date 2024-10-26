import { Guid } from 'guid-typescript'

export interface ReservationLeaseWriteDto {

    reservationId: Guid
    insuranceCompany: string
    policyEnds: string
    policyNo: string,
    flag: string
    registryPort: string
    registryNo: string
    boatType: string
    boatUsage: string
    netAmount: number
    vatAmount: number
    grossAmount: number

}
