import { LeaseRenewalBoatListVM } from "./lease-renewal-boat-list-vm"

export interface LeaseRenewalListVM {

    reservationId: string
    boat: LeaseRenewalBoatListVM
    leaseEnds: string

}
