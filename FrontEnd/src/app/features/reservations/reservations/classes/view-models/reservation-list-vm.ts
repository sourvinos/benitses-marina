import { ReservationListBerthVM } from './reservation-list-berth-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ReservationListVM {

    reservationId: string
    boatName: string
    ownerName: string
    boatLoa: string
    fromDate: string
    toDate: string
    berths: ReservationListBerthVM[]
    paymentStatus: SimpleEntity
    isFishingBoat: boolean
    isAthenian: boolean
    isDocked: boolean
    isDryDock: boolean
    isOverdue: boolean
    isRequest: boolean
    joinedBerths: string

}
