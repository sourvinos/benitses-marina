import { Guid } from 'guid-typescript'

export interface ReservationFishingLicenceDto {

    reservationId: Guid
    issuingAuthority: string
    licenceNo: string,
    licenceEnds: string

}
