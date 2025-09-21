import { Guid } from 'guid-typescript'

export interface ReservationFeeDto {

    reservationId: Guid
    netAmount: number
    discountPercent: number
    discountAmount: number
    netAmountAfterDiscount: number
    vatPercent: number
    vatAmount: number
    grossAmount: number
    isCash: boolean

}
