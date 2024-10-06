import { ReservationListPierVM } from './reservation-list-pier-vm'

export interface ReservationListVM {

    reservationId: string
    boatName: string
    customer: string
    loa: string
    fromDate: string
    toDate: string
    days: number
    isConfirmed: boolean
    isDocked: boolean
    paymentStatusDescription: string
    isLongTerm: boolean
    isOverdue: boolean
    piers: ReservationListPierVM[]
    joinedPiers: string

}
