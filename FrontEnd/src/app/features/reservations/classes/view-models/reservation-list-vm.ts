import { ReservationListBerthVM } from './reservation-list-berth-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ReservationListVM {

    reservationId: string
    fromDate: string
    toDate: string
    isDocked: boolean
    paymentStatus: SimpleEntity
    isDryDock: boolean
    isOverdue: boolean
    isGray: boolean
    boatName: string
    boatLoa: string
    ownerName: string
    berths: ReservationListBerthVM[]
    joinedBerths: string

}
