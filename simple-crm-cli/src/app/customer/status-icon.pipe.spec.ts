import { StatusIconPipe } from './status-icon.pipe';

describe('StatusIconPipe', () => {
  it('create an instance', () => {
    const pipe = new StatusIconPipe();
    expect(pipe).toBeTruthy();
  });
  it('Prospect should result in cancel', () => {
    const pipe = new StatusIconPipe(); // 1. SETUP: construct a new instance of the class.
    const x = pipe.transform('prospect'); // 2. INVOKE the method
    expect(x).toEqual('cancel'); // 3. VERIFY the result of the method matches what is expected.
  });
  it('prospect (lowercase) should result in cancel', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('prospect');
    expect(x).toEqual('cancel');
 });
 it('prOspEct (mixed case) should result in cancel', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('prOspEct');
    expect(x).toEqual('cancel');
 });
it('Ordered should result in full', () => {
   const pipe = new StatusIconPipe();
   const x = pipe.transform('Ordered');
   expect(x).toEqual('full');
});
it('ordered (lowercase) should result in full', () => {
  const pipe = new StatusIconPipe();
  const x = pipe.transform('Ordered');
  expect(x).toEqual('full');
});
it('orDereD (mixed case) should result in full', () => {
  const pipe = new StatusIconPipe();
  const x = pipe.transform('Ordered');
  expect(x).toEqual('full');
});
it('null should result in fallback value', () => {
  const pipe = new StatusIconPipe();
  const x = pipe.transform('null');
  expect(x).toEqual('checkout');
});
it('empty string should result in fallback value', () => {
  const pipe = new StatusIconPipe();
  const x = pipe.transform('');
  expect(x).toEqual('checkout');
});
});
