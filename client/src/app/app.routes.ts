import { Routes } from '@angular/router';
import { Home } from '../feature/home/home';
import { MemberList } from '../feature/members/member-list/member-list';
import { MemberDetailed } from '../feature/members/member-detailed/member-detailed';
import { Lists } from '../feature/lists/lists';
import { Messages } from '../feature/messages/messages';
import { authGuard } from '../core/guards/auth-guard';
import { NotFound } from '../shared/errors/not-found/not-found';
import { MemberProfile } from '../feature/members/member-profile/member-profile';
import { MemberPhotos } from '../feature/members/member-photos/member-photos';
import { MemberMessages } from '../feature/members/member-messages/member-messages';
import { memberResolver } from '../feature/members/member-resolver';
import { preventUnsavedChangesGuard } from '../core/guards/prevent-unsaved-changes-guard';

export const routes: Routes = [
    {path: '', component: Home},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [authGuard, preventUnsavedChangesGuard],
        children: [
            {path: 'members', component: MemberList},
            {
                path: 'members/:id', 
                resolve: {member: memberResolver},
                runGuardsAndResolvers: 'always',
                component: MemberDetailed,
                children: [
                    {path: '', redirectTo: 'profile', pathMatch: 'full'},
                    {path: 'profile', component: MemberProfile, title: 'Profile', canDeactivate: [preventUnsavedChangesGuard]},
                    {path: 'photos', component: MemberPhotos, title: 'Photos'},
                    {path: 'messages', component: MemberMessages, title: 'Messages'},
                ]
            },
            {path: 'lists', component: Lists},
            {path: 'messages', component: Messages},
        ]
    },
    {path: '**', component: NotFound},
];
