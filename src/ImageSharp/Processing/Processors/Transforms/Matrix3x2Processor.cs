﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System.Numerics;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace SixLabors.ImageSharp.Processing.Processors
{
    /// <summary>
    /// Provides methods to transform an image using a <see cref="Matrix3x2"/>.
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal abstract class Matrix3x2Processor<TPixel> : ImageProcessor<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        /// <summary>
        /// Gets the rectangle designating the target canvas.
        /// </summary>
        protected Rectangle CanvasRectangle { get; private set; }

        /// <summary>
        /// Creates a new target canvas to contain the results of the matrix transform.
        /// </summary>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="processMatrix">The processing matrix.</param>
        protected void CreateNewCanvas(Rectangle sourceRectangle, Matrix3x2 processMatrix)
        {
            Matrix3x2 sizeMatrix;
            this.CanvasRectangle = Matrix3x2.Invert(processMatrix, out sizeMatrix)
                ? ImageMaths.GetBoundingRectangle(sourceRectangle, sizeMatrix)
                : sourceRectangle;
        }

        /// <summary>
        /// Gets a transform matrix adjusted to center upon the target image bounds.
        /// </summary>
        /// <param name="source">The source image.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>
        /// The <see cref="Matrix3x2"/>.
        /// </returns>
        protected Matrix3x2 GetCenteredMatrix(ImageFrame<TPixel> source, Matrix3x2 matrix)
        {
            var translationToTargetCenter = Matrix3x2.CreateTranslation(-this.CanvasRectangle.Width * .5F, -this.CanvasRectangle.Height * .5F);
            var translateToSourceCenter = Matrix3x2.CreateTranslation(source.Width * .5F, source.Height * .5F);
            return (translationToTargetCenter * matrix) * translateToSourceCenter;
        }
    }
}
