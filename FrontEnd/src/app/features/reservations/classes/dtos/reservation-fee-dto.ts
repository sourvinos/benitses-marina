import { Guid } from 'guid-typescript'

export interface ReservationFeeDto {

    reservationId: Guid
    netAmount: number
    vatAmount: number
    grossAmount: number
    isCash: boolean

}
