import { ReservationListPierVM } from './reservation-list-pier-vm'

export interface ReservationListVM {

    reservationId: string
    boatName: string
    customer: string
    length: string
    fromDate: string
    toDate: string
    days: number
    isLongTerm: boolean
    validThruDate: string
    piers: ReservationListPierVM[]
    joinedPiers: string

}
