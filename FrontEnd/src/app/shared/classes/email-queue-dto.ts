export interface EmailQueueDto {

    initiator: string
    entityId: string
    filenames: string
    priority: number
    isSent: boolean

}
