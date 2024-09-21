import { ReservationListPierVM } from './reservation-list-pier-vm'

export interface ReservationListVM {

    reservationId: string
    boatName: string
    boatLength: string
    fromDate: string
    toDate: string
    stayDuration: number
    piers: ReservationListPierVM[]
    joinedPiers: string

}
