import { BookingListPierVM } from './booking-list-pier-vm'

export interface BookingListVM {

    bookingId: string
    boatName: string
    boatLength: string
    fromDate: string
    toDate: string
    stayDuration: number
    piers: BookingListPierVM[]
    joinedPiers: string

}
