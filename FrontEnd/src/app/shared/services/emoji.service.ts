import { Injectable } from '@angular/core'

@Injectable({ providedIn: 'root' })

export class EmojiService {

    public getEmoji(emoji: string): string {
        switch (emoji) {
            case 'blue-box': return '🟦'
            case 'edit': return '✒️'
            case 'error': return '❌'
            case 'green-box': return '🟩'
            case 'null': return '🚫'
            case 'red-box': return '🟥'
            case 'remarks': return '🔔'
            case 'wildcard': return '⭐'
            case 'yellow-box': return '🟨'
            case 'notActive': return '☢️ '
            case 'active': return '🟢 '
            case 'ok': return '✔️'
        }

    }

}
