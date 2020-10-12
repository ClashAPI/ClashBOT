import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPatchesComponent } from './admin-patches.component';

describe('AdminPatchesComponent', () => {
  let component: AdminPatchesComponent;
  let fixture: ComponentFixture<AdminPatchesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminPatchesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminPatchesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
