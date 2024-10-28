import { Guid } from 'guid-typescript'

export interface ReservationInsuranceDto {

    reservationId: Guid
    insuranceCompany: string
    policyNo: string,
    policyEnds: string

}
