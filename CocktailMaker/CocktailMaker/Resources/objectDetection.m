function [y] = objectDetection(path, data) 

bottles = char(data);
[m,n] = size(data);
res = '';

% loading the target
targetImage = imread(path);
%targetImage = imrotate(targetImage, 180);
targetImage = rgb2gray(targetImage);
    
for i = 1: m;
    
	% loading the object we are looking for
    path2 = bottles(i,:);
    compareImage = imread(path2);
    compareImage = rgb2gray(compareImage);

    %step 1 - finding detected features using SURF, and selecting the strongest
    objectPoints = detectSURFFeatures(compareImage);
    targetPoint = detectSURFFeatures(targetImage);

    selectStrongest(objectPoints, 100);
    selectStrongest(targetPoint, 300);

    %Step 3: Extract features using the strongest points

    [objectFeatures, objectPoints] = extractFeatures(compareImage, objectPoints);
    [sceneFeatures, targetPoint] = extractFeatures(targetImage, targetPoint);

    %Step 4: Find matching between target and object using their descriptors.
    matchPoints = matchFeatures(objectFeatures, sceneFeatures);

    matchedBoxPoints = objectPoints(matchPoints(:, 1), :);
    matchedScenePoints = targetPoint(matchPoints(:, 2), :);

    try
        %Step 5: check the treshhold (on this case- 5)
        [ignoreThis1, inlierPoints, ignoreThis2] = estimateGeometricTransform(matchedBoxPoints, matchedScenePoints, 'affine');
        if (length(inlierPoints) > 5);
            res = [res bottles(i,:)];
        end;
    catch
        
    end
end;

y = res;