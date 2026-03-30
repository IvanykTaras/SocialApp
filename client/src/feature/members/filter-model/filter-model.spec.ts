import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterModel } from './filter-model';

describe('FilterModel', () => {
  let component: FilterModel;
  let fixture: ComponentFixture<FilterModel>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FilterModel],
    }).compileComponents();

    fixture = TestBed.createComponent(FilterModel);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
