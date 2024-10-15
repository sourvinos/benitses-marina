import { ReservationListBerthVM } from './reservation-list-berth-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ReservationListVM {

    reservationId: string
    boatName: string
    customer: string
    loa: string
    fromDate: string
    toDate: string
    days: number
    isDocked: boolean
    paymentStatus: SimpleEntity
    isLongTerm: boolean
    isOverdue: boolean
    berths: ReservationListBerthVM[]
    joinedBerths: string

}
