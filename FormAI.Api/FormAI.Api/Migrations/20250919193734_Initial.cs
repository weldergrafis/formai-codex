using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GalleryId = table.Column<long>(type: "bigint", nullable: false),
                    LocalPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUploaded = table.Column<bool>(type: "bit", nullable: false),
                    IsResized = table.Column<bool>(type: "bit", nullable: false),
                    IsFaceDetected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photo_Gallery_GalleryId",
                        column: x => x.GalleryId,
                        principalTable: "Gallery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Face",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoId = table.Column<long>(type: "bigint", nullable: false),
                    PersonId = table.Column<long>(type: "bigint", nullable: true),
                    ComparisonOrder = table.Column<long>(type: "bigint", nullable: false),
                    NeurotecOrder = table.Column<int>(type: "int", nullable: false),
                    Template = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Pitch = table.Column<double>(type: "float", nullable: false),
                    Roll = table.Column<double>(type: "float", nullable: false),
                    Yaw = table.Column<double>(type: "float", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    GenderConfidence = table.Column<double>(type: "float", nullable: false),
                    LeftEyeCenterX = table.Column<double>(type: "float", nullable: false),
                    LeftEyeCenterY = table.Column<double>(type: "float", nullable: false),
                    LeftEyeCenterConfidence = table.Column<double>(type: "float", nullable: false),
                    RightEyeCenterX = table.Column<double>(type: "float", nullable: false),
                    RightEyeCenterY = table.Column<double>(type: "float", nullable: false),
                    RightEyeCenterConfidence = table.Column<double>(type: "float", nullable: false),
                    BothEyesCenterX = table.Column<double>(type: "float", nullable: false),
                    BothEyesCenterY = table.Column<double>(type: "float", nullable: false),
                    BothEyesCenterConfidence = table.Column<double>(type: "float", nullable: false),
                    NoseTipX = table.Column<double>(type: "float", nullable: false),
                    NoseTipY = table.Column<double>(type: "float", nullable: false),
                    NoseTipConfidence = table.Column<double>(type: "float", nullable: false),
                    MouthCenterX = table.Column<double>(type: "float", nullable: false),
                    MouthCenterY = table.Column<double>(type: "float", nullable: false),
                    MouthCenterConfidence = table.Column<double>(type: "float", nullable: false),
                    Quality = table.Column<int>(type: "int", nullable: false),
                    DetectionConfidence = table.Column<double>(type: "float", nullable: false),
                    Occlusion = table.Column<int>(type: "int", nullable: false),
                    Resolution = table.Column<int>(type: "int", nullable: false),
                    MotionBlur = table.Column<int>(type: "int", nullable: false),
                    CompressionArtifacts = table.Column<int>(type: "int", nullable: false),
                    Overexposure = table.Column<int>(type: "int", nullable: false),
                    Underexposure = table.Column<int>(type: "int", nullable: false),
                    GrayscaleDensity = table.Column<int>(type: "int", nullable: false),
                    Sharpness = table.Column<int>(type: "int", nullable: false),
                    Contrast = table.Column<int>(type: "int", nullable: false),
                    BackgroundUniformity = table.Column<int>(type: "int", nullable: false),
                    Saturation = table.Column<int>(type: "int", nullable: false),
                    Noise = table.Column<int>(type: "int", nullable: false),
                    WashedOut = table.Column<int>(type: "int", nullable: false),
                    Pixelation = table.Column<int>(type: "int", nullable: false),
                    Interlace = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Pose = table.Column<int>(type: "int", nullable: false),
                    EyesOpen = table.Column<int>(type: "int", nullable: false),
                    DarkGlasses = table.Column<int>(type: "int", nullable: false),
                    Glasses = table.Column<int>(type: "int", nullable: false),
                    MouthOpen = table.Column<int>(type: "int", nullable: false),
                    Beard = table.Column<int>(type: "int", nullable: false),
                    Mustache = table.Column<int>(type: "int", nullable: false),
                    HeadCovering = table.Column<int>(type: "int", nullable: false),
                    HeavyFrameGlasses = table.Column<int>(type: "int", nullable: false),
                    LookingAway = table.Column<int>(type: "int", nullable: false),
                    RedEye = table.Column<int>(type: "int", nullable: false),
                    FaceDarkness = table.Column<int>(type: "int", nullable: false),
                    SkinTone = table.Column<int>(type: "int", nullable: false),
                    SkinReflection = table.Column<int>(type: "int", nullable: false),
                    GlassesReflection = table.Column<int>(type: "int", nullable: false),
                    FaceMask = table.Column<int>(type: "int", nullable: false),
                    AdditionalFacesDetected = table.Column<int>(type: "int", nullable: false),
                    GenderMale = table.Column<int>(type: "int", nullable: false),
                    GenderFemale = table.Column<int>(type: "int", nullable: false),
                    Smile = table.Column<int>(type: "int", nullable: false),
                    TokenImageQuality = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Face", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Face_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Face_Photo_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FaceComparison",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Face1Id = table.Column<long>(type: "bigint", nullable: false),
                    Face2Id = table.Column<long>(type: "bigint", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    IsSamePerson = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaceComparison", x => x.Id);
                    table.CheckConstraint("CK_FaceComparison_OrderedPair", "[Face1Id] < [Face2Id]");
                    table.ForeignKey(
                        name: "FK_FaceComparison_Face_Face1Id",
                        column: x => x.Face1Id,
                        principalTable: "Face",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FaceComparison_Face_Face2Id",
                        column: x => x.Face2Id,
                        principalTable: "Face",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Face_ComparisonOrder",
                table: "Face",
                column: "ComparisonOrder",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Face_PersonId",
                table: "Face",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Face_PhotoId",
                table: "Face",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceComparison_Face1Id_Face2Id",
                table: "FaceComparison",
                columns: new[] { "Face1Id", "Face2Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FaceComparison_Face2Id",
                table: "FaceComparison",
                column: "Face2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_GalleryId",
                table: "Photo",
                column: "GalleryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaceComparison");

            migrationBuilder.DropTable(
                name: "Face");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropTable(
                name: "Gallery");
        }
    }
}
