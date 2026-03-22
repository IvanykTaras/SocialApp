import { Component, inject, OnInit, signal } from '@angular/core';
import { MemberService } from '../../../core/services/member-service';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Member, Photo } from '../../../types/member';
import { AsyncPipe } from '@angular/common';
import { ImageUpload } from "../../../shared/image-upload/image-upload";
import { AccountService } from '../../../core/services/account-service';
import { User } from '../../../types/user';
import { StarButton } from "../../../shared/start-button/start-button";
import { DeleteButton } from "../../../shared/delete-button/delete-button";

@Component({
  selector: 'app-member-photos',
  imports: [ImageUpload, StarButton, DeleteButton],
  templateUrl: './member-photos.html',
  styleUrl: './member-photos.css',
})
export class MemberPhotos implements OnInit {
  protected memberServices = inject(MemberService);
  protected accountServices = inject(AccountService);
  private route = inject(ActivatedRoute);
  protected photos = signal<Photo[]>([]);
  protected loading = signal<boolean>(false);

  ngOnInit(): void {
    const memberId = this.route.parent?.snapshot.paramMap.get('id');
    if (memberId) {
      this.memberServices.getMemberPhotos(memberId).subscribe({
        next: photos => this.photos.set(photos)
      });
    }
  }

  onUploadImage(file: File) {
    this.loading.set(true);
    this.memberServices.uploadFile(file).subscribe({
      next: photo => {
        this.memberServices.editMode.set(false);
        this.loading.set(false);
        this.photos.update(photos => [...photos, photo]);
        if(!this.memberServices.member()?.imageUrl) {
          this.setMainLocalPhoto(photo);
        }
      },
      error: err => {
        console.error('Error uploading image:', err);
        this.loading.set(false);
      }
    })
  }

  setMainPhoto(photo: Photo) {
    this.memberServices.setMainPhoto(photo).subscribe({
      next: () => {
        this.setMainLocalPhoto(photo);
      }
    })
  }

  deletePhoto(photoId: number) {
    this.memberServices.deletePhoto(photoId).subscribe({
      next: () => {
        this.photos.update(photos => photos.filter(x => x.id !== photoId))
      }
    })
  }

  private setMainLocalPhoto(photo: Photo) {
    const currentUser = this.accountServices.currentUser();
    if (currentUser) currentUser.imageUrl = photo.url;
    this.accountServices.setCurrentUser(currentUser as User);
    this.memberServices.member.update(member => ({
      ...member,
      imageUrl: photo.url
    }) as Member)
  }




}
