import { Directive, HostListener, Input } from '@angular/core'

@Directive({ selector: '[inputTabStop]' })

export class InputTabStopDirective {

    @Input('inputTabStop') format: any
    @Input('dataTabIndex') x: any

    @HostListener('keydown', ['$event']) onkeydown(event: { target: { getAttribute: (arg0: string) => string; }; key: string; preventDefault: () => void; }): any {
        if (event.target.getAttribute('type') === 'number' && (event.key === 'ArrowDown' || event.key === 'ArrowUp')) {
            event.preventDefault()
        }
    }

    @HostListener('keyup', ['$event']) onkeyup(event: { preventDefault: () => void; key: string; target: { getAttribute: (arg0: string) => any; }; }): any {

        const elements = Array.prototype.slice.apply(document.querySelectorAll('input[dataTabIndex]'))

        if (document.getElementsByClassName('mat-mdc-option').length == 0) {

            if (event.key === 'Enter' || event.key === 'ArrowDown') {
                let nextTab = +(event.target.getAttribute('dataTabIndex')) + 1
                for (let i = elements.length; i--;) {
                    if (nextTab > elements.length) { nextTab = 1 }
                    if (+(elements[i].getAttribute('dataTabIndex')) === nextTab && !elements[i].getAttribute('disabled')) {
                        elements[i].focus()
                        elements[i].select()
                        break
                    }
                }
            }

            if (event.key === 'ArrowUp') {
                let previousTab = +(event.target.getAttribute('dataTabIndex')) - 1
                for (let i = elements.length; i--;) {
                    if (previousTab === 0) { previousTab = elements.length }
                    if (+(elements[i].getAttribute('dataTabIndex')) === previousTab) {
                        elements[i].focus()
                        elements[i].select()
                        break
                    }
                }

            }

        }

    }

}
