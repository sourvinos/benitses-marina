import { ReservationListPierVM } from './reservation-list-pier-vm'

export interface ReservationListVM {

    reservationId: string
    boatName: string
    length: string
    fromDate: string
    toDate: string
    days: number
    piers: ReservationListPierVM[]
    joinedPiers: string

}
