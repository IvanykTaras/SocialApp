import { Routes } from '@angular/router';
import { Home } from '../feature/home/home';
import { MemberList } from '../feature/members/member-list/member-list';
import { MemberDetailed } from '../feature/members/member-detailed/member-detailed';
import { Lists } from '../feature/lists/lists';
import { Messages } from '../feature/messages/messages';
import { authGuard } from '../core/guards/auth-guard';
import { NotFound } from '../shared/errors/not-found/not-found';

export const routes: Routes = [
    {path: '', component: Home},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [authGuard],
        children: [
            {path: 'members', component: MemberList},
            {path: 'members/:id', component: MemberDetailed},
            {path: 'lists', component: Lists},
            {path: 'messages', component: Messages},
        ]
    },
    {path: '**', component: NotFound},
];
