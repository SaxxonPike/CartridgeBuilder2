# CartridgeBuilder2

Second iteration of the Commodore 64 cartridge builder. Designed for building EasyFlash
cartridge images, but it could be repurposed to generate images for other hardware.

### Requirements

- .NET Standard 2.0 (for library), .NET Core 3.0 (for CLI)

### Usage

Command line: `CartridgeBuilder2 <config>`

`<config>` is the JSON configuration file that contains information about files, patches and 
tables to include when generating cartridge images.

### Configuration JSON hierarchy

- Root element (object)
  - `Exrom` (bool)
    - Exrom pin state (active true)
    - Defaults true (for EasyFlash)
  - `Game` (bool)
    - Game pin state (active true)
    - Defaults false (for EasyFlash)
  - `Hardware` (int)
    - Hardware ID. See documentation on the CRT format for values to use.
    - Defaults to EasyFlash (0x20)
  - `Capacity` (int)
    - Capacity of the output ROM, in bytes.
    - Defaults to 1 megabyte (for EasyFlash)
  - `Name` (string)
    - Name to embed into the CRT file.
  - `Files` (array of objects)
    - `LoadAddress` (int)
      - 16-bit load address. 
    - `Name` (string)
      - Name of the file for use in tables.
      - This will soon be converted to PETSCII.
    - `Path` (string, required)
      - Path to a file or directory.
      - If this is a path to a directory, all properties in this file block are applied to each
        file in the directory. Directory use can be combined with the Name property, but probably 
        is not a very good idea unless no file name tables will be built. If no Name property is
        specified, it is derived from the input file name.
  - `Patches` (array of objects)
    - `Bank` (int, required)
      - Bank number to write the patch to.
    - `Offset` (int, required)
      - 16-bit offset within the bank. Use values 0000-3FFF hex.
    - `WrapStrategy` (see below)
      - Determines bank wrapping.
    - `Path` (string, required)
      - Path to a file. Don't use directories here.
  - `Tables` (array of objects)
    - `Bank` (int, required)
      - Bank number to write the table to.
    - `Offset` (int, required)
      - 16-bit offset within the bank. Use values 0000-3FFF hex.
    - `Length` (int)
      - Maximum length of table data in bytes.
      - If not specified, the table will automatically be resized to the number of input files.
    - `Index` (int)
      - When using table types that require an additional parameter, this is the parameter value.
        (example: which character in a filename to store)
    - `WrapStrategy` (see below)
      - Determines bank wrapping.
    - `Type` (see below, required)
      - Determines what kind of data is written to this table.
  - `Fills` (array of objects)
    - `Bank` (int, required)
      - Bank number to write the data to.
    - `Offset` (int, required)
      - 16-bit offset within the bank. Use values 0000-3FFF hex.
    - `Length` (int)
      - Length of data to generate.
    - `WrapStrategy` (see below)
      - Determines bank wrapping.

##### Wrap strategies

- `Both`: use both the 8000 and A000 memory mapped regions. (Default)
- `Low`: use only the 8000 memory mapped region.
- `High`: use only the A000 memory mapped region.

##### Table types

- `Bank`: bank number where a file is stored.
- `Name`: output file name, determined by properties in the Files section.
- `OffsetLow`: low 8 bits of the offset within the bank.
- `OffsetHigh`: high 8 bits of the offset within the bank.
- `StartAddressLow`: low 8 bits of the system-mapped address within the bank.
- `StartAddressHigh`: high 8 bits of the system-mapped address within the bank.
- `LoadAddressLow`: low 8 bits of the load address.
- `LoadAddressHigh`: high 8 bits of the load address.
- `LengthLow`: low 8 bits of the data length.
- `LengthHigh`: high 8 bits of the data length.
- `OffsetReset`: see the Bank Wrapping section for more info.
- `Compressed`: determines the compression type.

##### Bank wrapping

When data reaches the end of storage space specified by the wrapping strategy while being written,
additional information can be stored to reflect at which page to perform a wrap, and which page
to wrap back to.

- `Both`: reset is 0x80, match is 0xC0
- `Low`: reset is 0x80, match is 0xA0
- `High`: reset is 0xA0, match is 0xC0

### Technology

- Target frameworks
  - CLI application
    - .NET Core 3.0
  - Library
    - .NET Standard 2.0

- Compatible build processes
  - `dotnet build`
  - JetBrains Rider
  
### Contributing

- Make a pull request. If possible, please also provide unit tests for any code that has changed,
  ensure that all unit tests pass, and that existing unit tests are not altered unless new tests
  render them irrelevant or incompatible.
- Understand that this project uses the MIT license and that all new code must be compatible with
  this license.

### Todo

- The ability to specify which wrapping strategies the packer will consider (not all filesystems
  will support everything, so...)

### License

MIT. See LICENSE for details.